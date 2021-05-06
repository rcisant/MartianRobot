$rsgName = 'swacoefooprojectlab'
$subscriptionName = 'sgs-sandboxdev-01'
$regionName = 'West Europe'

if ((az group exists -n $rsgName) -eq $false) {
    az group create --name $rsgName --location $regionName --subscription $subscriptionName
}

az deployment group create --resource-group $rsgName `
    --template-file 'arm-main.json' `
    --parameters 'arm-main.parameters.json'
