# Virtual network - Peering

# Purpose of this demo
# How the demo organized
- Creates 2 virual networks and 1 subnet each
- Establish peering
- Deploy 2 Linux VMs one each into subnets
  - PrimaryVM to have public IP address
  - SecondaryVM only with internal IP
  
# Demos

## Normal working
- Log into the PrimaryVM
- Ping the SecondaryVM

### Expected
- Ping will succeed if the peering is set up right

## Negative scenario
- Remove the peering
- Ping the SecondaryVM
### Expected
- Ping will wait
