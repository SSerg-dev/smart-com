data "yandex_vpc_network" "network" { 
  name = var.network-name
  folder_id = data.yandex_resourcemanager_folder.folder_net.id
}

resource "yandex_vpc_gateway" "nat-gateway" {
  folder_id = data.yandex_resourcemanager_folder.folder_net.id
  name = var.natgw-name
  shared_egress_gateway {}
}

resource "yandex_vpc_route_table" "rt" {
  folder_id = data.yandex_resourcemanager_folder.folder_net.id
  name       = var.rt-name
  network_id = data.yandex_vpc_network.network.id
  static_route {
    destination_prefix = "192.168.10.177/32"
    next_hop_address   = "172.20.0.11"
  }
  static_route {
    destination_prefix = "192.168.10.39/32"
    next_hop_address   = "172.20.0.11"
  }
  static_route {
    destination_prefix = "192.168.10.171/32"
    next_hop_address   = "172.20.0.11"
  }
  static_route {
    destination_prefix = "0.0.0.0/0"
    gateway_id         = "${yandex_vpc_gateway.nat-gateway.id}"
  }
}

resource "yandex_vpc_subnet" "subnet" {
  name           = var.subnet-name
  zone           = var.zone
  network_id     = data.yandex_vpc_network.network.id
  v4_cidr_blocks = var.subnet-cidr
  folder_id      = data.yandex_resourcemanager_folder.folder_net.id
  route_table_id = "${yandex_vpc_route_table.rt.id}"
}