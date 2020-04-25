$RGName = 'to-delete-vnet-peering'
New-AzResourceGroupDeployment -Name VMsDeployment1 -ResourceGroupName $RGName `
  -TemplateFile ./azuredeploy-vms.json `
  -TemplateParameterFile ./azuredeploy-vms.parameters.json


  #Remove-AzResourceGroupDeployment -Name VMsDeployment1 -ResourceGroupName $RGName