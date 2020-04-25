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
#Connect-AzAccount
$RGName = 'to-delete-vnet-peering'
$imageName = "Win2016Datacenter"
$imageName = "UbuntuLTS"
$RG = Get-AzResourceGroup -Name $RGName
if ($RG) {
    "Resource group exists"
}
else {
    $RG = New-AzResourceGroup -Name $RGName -Location eastus -Force
}
$cred = Get-Credential -Message "Enter a username and password for the virtual machine." -UserName 'testadmin'
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
    PublicIpAddressName = 'secondaryVMPublicIp'
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
