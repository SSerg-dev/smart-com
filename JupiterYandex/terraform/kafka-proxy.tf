# resource "yandex_compute_instance" "kafka-proxy" {
#   folder_id = yandex_resourcemanager_folder.folder.id
#   name        = var.kafka-proxy.name
#   platform_id = var.kafka-proxy.platform_id
#   zone        = var.zone

#   resources {
#     cores  = var.kafka-proxy.cpu
#     core_fraction = var.kafka-proxy.cpu_fraction
#     memory = var.kafka-proxy.memory
#   }
#   boot_disk {
#     initialize_params {
#       image_id = "fd8haecqq3rn9ch89eua"
#       size = var.kafka-proxy.disk
#       type = "network-ssd"
#     }
#   }
#   network_interface {
#     subnet_id = "${yandex_vpc_subnet.subnet.id}"
#   }
#   scheduling_policy {
#       preemptible = true
#   }
#   allow_stopping_for_update = true
#   metadata = {
#     ssh-keys = var.kafka-proxy.ssh-key
#   }
# }