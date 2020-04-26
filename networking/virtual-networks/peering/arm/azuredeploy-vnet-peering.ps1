$RGName = 'to-delete-vnet-peering'
New-AzResourceGroupDeployment -Name VMPeeringDeployment1 -ResourceGroupName $RGName `
  -TemplateFile ./azuredeploy-vnet-peering.json `
  -TemplateParameterFile ./azuredeploy-vnet-peering.parameters.json