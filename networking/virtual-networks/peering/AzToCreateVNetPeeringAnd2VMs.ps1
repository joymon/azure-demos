####################### Params start ##########################3
#Connect-AzAccount
$RGName = 'to-delete-vnet-peering'
$primaryVNetName = "primaryNetwork"
$secondaryVNetName = "secondaryNetwork"
$imageName = "UbuntuLTS"

$cred = Get-Credential -Message "Enter a username and password for the virtual machine." -UserName 'testadmin'
#################### Params End ###########
################## Functions start ###########
function New-AzVMIfNotExists{
    param($name,$vmparams)
    $VM = Get-azvm -name $name
    if ($VM) {
        "$name VMExists"
    }
    else {
        "Creating VM name:" + $vmparams.Name
        $VM = New-AzVM @vmparams
        $VM
    }
    return $VM
}
################## Functions End ###########
$RG = Get-AzResourceGroup -Name $RGName
if ($RG) {
    "Resource group exists"
}
else {
    $RG = New-AzResourceGroup -Name $RGName -Location eastus -Force
}
################# Create VMs ##############
$primaryVMParams = @{
    ResourceGRoupName   = $RGName
    Name                = 'PrimaryVM'
    Location            = "EastUS"
    ImageName           = $imageName
    PublicIpAddressName = 'primaryVMPublicIp'
    Credential          = $cred
    OpenPorts           = 3389, 22
    Size                = "Standard_A1_v2"
    VirtualNetworkName  = 'primaryNetwork'
    AddressPrefix ="192.168.0.0/24"
    SubNetName          = 'primarySubNet'
    SubnetAddressPrefix="192.168.0.0/28"
}
$primaryVM = New-AzVMIfNotExists -name 'PrimaryVM' -vmparams $primaryVMParams
$primaryVM
$secondaryVMParams = @{
    ResourceGRoupName   = $RGName
    Name                = 'SecondaryVM'
    Location            = "EastUS"
    ImageName           = $imageName
    Credential          = $cred
    OpenPorts           = 3389, 22
    Size                = "Standard_A1_v2"
    VirtualNetworkName  = 'secondaryNetwork'
    AddressPrefix ="192.168.1.0/24"
    SubNetName          = 'secondarySubNet'
    SubnetAddressPrefix="192.168.1.0/28"
}
$secondaryVM=New-AzVMIfNotExists -name 'SecondaryVM' -vmparams $secondaryVMParams
$secondaryVM
################## VNet Peering #########################3
$primaryVNet = Get-AzVirtualNetwork -Name $primaryVNetName
$secondaryVNet = Get-AzVirtualNetwork -Name $secondaryVNetName
$peeringPrimary2Secondary= Get-AzVirtualNetworkPeering -ResourceGroupName $RGName -VirtualNetworkName $primaryVNetName
if($peeringPrimary2Secondary) {
    "Peering exists"
}
else {
    "Adding primary to secondary"
    $peeringPrimary2Secondary = Add-AzVirtualNetworkPeering -Name 'primary2secondary' -VirtualNetwork $primaryVNet -RemoteVirtualNetworkId $secondaryVNet.Id
}