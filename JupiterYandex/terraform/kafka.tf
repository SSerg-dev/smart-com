resource "yandex_mdb_kafka_cluster" "kafka" {
  name        = var.kafka.name
  folder_id = yandex_resourcemanager_folder.folder.id
  environment = "PRODUCTION"
  network_id  = data.yandex_vpc_network.network.id
  subnet_ids  = ["${yandex_vpc_subnet.subnet.id}"]
  config {
    version          = var.kafka.version
    brokers_count    = 1
    zones            = [var.zone]
    assign_public_ip = false
    unmanaged_topics = false
    schema_registry  = false
    kafka {
      kafka_config {}
      resources {
        resource_preset_id = var.kafka.preset_id
        disk_type_id       = "network-ssd"
        disk_size          = var.kafka.disk
      }
    }
  }
  maintenance_window {
    type = "WEEKLY"
    day  = "SUN"
    hour = "1"
  }
}

resource "yandex_mdb_kafka_user" user_events {
  cluster_id = yandex_mdb_kafka_cluster.kafka.id
  name       = var.kafka.user
  password   = var.kafka.pass
  # permission {
  #   topic_name = "ENBL_IN"
  #   role       = ["ACCESS_ROLE_CONSUMER", "ACCESS_ROLE_PRODUCER"]
  # }
  # permission {
  #   topic_name = "YRLAM_IN"
  #   role       = ["ACCESS_ROLE_CONSUMER", "ACCESS_ROLE_PRODUCER"]
  # }
}