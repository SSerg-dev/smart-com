resource "yandex_vpc_network" "network" { 
  name = var.network-name
  folder_id = yandex_resourcemanager_folder.folder.id
}

resource "yandex_vpc_subnet" "subnet" {
  name           = var.subnet-name
  zone           = var.zone
  network_id     = yandex_vpc_network.network.id
  v4_cidr_blocks = var.subnet-cidr
  folder_id = yandex_resourcemanager_folder.folder.id
}