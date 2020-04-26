####################### Params start ##########################3
#Connect-AzAccount
$region = "EastUS"
$RGName = 'to-delete-vnet-peering'
$primaryVNetName = "primaryNetwork"
$secondaryVNetName = "secondaryNetwork"
$imageName = "UbuntuLTS"

$cred = Get-Credential -Message "Enter a username and password for the virtual machine." -UserName 'testadmin'
#################### Params End ###########
################## Functions start ###########
function New-AzVMIfNotExists {
    param([string]$name, $vmparams)
    $VM = Get-azvm -name $name
    if ($VM) {
        Write-Host -ForegroundColor Green "VM $name already exists"
    }
    else {
        "Creating VM name:" + $vmparams.Name
        $VM = New-AzVM @vmparams
        Write-Host -ForegroundColor Green "VM $($VM.Name) created"
    }
    return $VM
}
function Add-AzVirtualNetworkPeeringIfNotExists {
    param([String] $ResourceGroupName, [string] $name, $virtualNetwork, $RemoteNetwork )
    $peering = Get-AzVirtualNetworkPeering -ResourceGroupName $ResourceGroupName -VirtualNetwork $virtualNetwork.Name -Name $name -ErrorAction SilentlyContinue
    if ($peering) {
        Write-Host -ForegroundColor Green "VNet peering $name already exists from $($virtualNetwork.name) to $($RemoteNetwork.Name). Status: $($peering.PeeringState)"
    }
    else {
        Write-Host "Adding peering from $($virtualNetwork.name) to $($RemoteNetwork.Name)"
        $peering = Add-AzVirtualNetworkPeering -Name $name -VirtualNetwork $virtualNetwork -RemoteVirtualNetworkId $RemoteNetwork.Id
    }
    return $peering
}
################## Functions End ###########
$RG = Get-AzResourceGroup -Name $RGName
if ($RG) {
    Write-Host -ForegroundColor Green "Resource group exists"
}
else {
    $RG = New-AzResourceGroup -Name $RGName -Location eastus -Force
}
################# Create VMs ##############
$primaryVMParams = @{
    ResourceGRoupName   = $RGName
    Name                = 'PrimaryVM'
    Location            = $region
    ImageName           = $imageName
    PublicIpAddressName = 'PrimaryVMPublicIP'
    Credential          = $cred
    OpenPorts           = 3389, 22
    Size                = "Standard_A1_v2"
    VirtualNetworkName  = 'primaryNetwork'
    AddressPrefix       = "192.168.0.0/24"
    SubNetName          = 'primarySubNet'
    SubnetAddressPrefix = "192.168.0.0/28"
}
$primaryVM = New-AzVMIfNotExists -name 'PrimaryVM' -vmparams $primaryVMParams
$secondaryVMParams = @{
    ResourceGRoupName   = $RGName
    Name                = 'SecondaryVM'
    Location            = $region
    ImageName           = $imageName
    PublicIpAddressName = 'primaryVMPublicIp'
    Credential          = $cred
    OpenPorts           = 3389, 22
    Size                = "Standard_A1_v2"
    VirtualNetworkName  = 'secondaryNetwork'
    AddressPrefix       = "192.168.1.0/24"
    SubNetName          = 'secondarySubNet'
    SubnetAddressPrefix = "192.168.1.0/28"
}
$secondaryVM = New-AzVMIfNotExists -name 'SecondaryVM' -vmparams $secondaryVMParams
################## VNet Peering #########################3
$primaryVNet = Get-AzVirtualNetwork -Name $primaryVNetName
$secondaryVNet = Get-AzVirtualNetwork -Name $secondaryVNetName

$peeringPrimary2Secondary = Add-AzVirtualNetworkPeeringIfNotExists -ResourceGroupName $RGName -Name 'primary2secondary' -VirtualNetwork $primaryVNet -remoteNetwork $secondaryVNet
$peeringSecondary2Primary = Add-AzVirtualNetworkPeeringIfNotExists -ResourceGroupName $RGName -Name 'secondary2primary' -VirtualNetwork $secondaryVNet -remoteNetwork $primaryVNet

"Completed"
