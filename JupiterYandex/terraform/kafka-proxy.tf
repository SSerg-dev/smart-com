data "yandex_compute_image" "container-optimized-image" {
  family = "container-optimized-image"
}

resource "yandex_iam_service_account" "kafka-proxy-sa" {
 name        = var.kafka-proxy.sa-name
 description = "Service account Kafka Proxy"
 folder_id   = yandex_resourcemanager_folder.folder.id
}

resource "yandex_resourcemanager_folder_iam_binding" "kafka-proxy-sa-pull" {
 folder_id = yandex_resourcemanager_folder.folder.id
 role      = "container-registry.images.puller"
 members   = [
   "serviceAccount:${yandex_iam_service_account.kafka-proxy-sa.id}"
 ]
}

resource "yandex_compute_instance" "kafka-proxy" {
  folder_id   = yandex_resourcemanager_folder.folder.id
  name        = var.kafka-proxy.name
  platform_id = var.kafka-proxy.platform_id
  zone        = var.zone
  service_account_id = yandex_iam_service_account.kafka-proxy-sa.id
  resources {
    cores  = var.kafka-proxy.cpu
    core_fraction = var.kafka-proxy.cpu_fraction
    memory = var.kafka-proxy.memory
  }
  boot_disk {
    initialize_params {
      image_id = data.yandex_compute_image.container-optimized-image.id
      size = var.kafka-proxy.disk
      type = "network-ssd"
    }
  }
  network_interface {
    subnet_id = "${yandex_vpc_subnet.subnet.id}"
  }
  scheduling_policy {
      preemptible = true
  }
  allow_stopping_for_update = true
  metadata = {
    docker-compose = templatefile("kafka-proxy-compose.yaml", {
        kafkaNodeUri = element(yandex_mdb_kafka_cluster.kafka.host[*].name, 0)
        kafkaProxyImage = var.kafka-proxy.image
        kafkaUser = var.kafka.user
        kafkaPass = var.kafka.pass
    })
    user-data = file("kafka-proxy-config.yaml")
  }
}