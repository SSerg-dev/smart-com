
resource "yandex_iam_service_account" "k8s-sa" {
 name        = var.k8s-sa-name
 description = "Service account k8s"
 folder_id = yandex_resourcemanager_folder.folder.id
}

resource "yandex_resourcemanager_folder_iam_binding" "editor" {
 folder_id = yandex_resourcemanager_folder.folder.id
 role      = "editor"
 members   = [
   "serviceAccount:${yandex_iam_service_account.k8s-sa.id}"
 ]
}

resource "yandex_iam_service_account" "k8s-nsa" {
 name        = var.k8s-nsa-name
 description = "Service account k8s for nodes"
 folder_id = yandex_resourcemanager_folder.folder.id
}

resource "yandex_resourcemanager_folder_iam_binding" "images-puller" {
 folder_id = yandex_resourcemanager_folder.folder.id
 role      = "container-registry.images.puller"
 members   = [
   "serviceAccount:${yandex_iam_service_account.k8s-nsa.id}"
 ]
}

resource "yandex_kms_symmetric_key" "k8s-kms-key" {
  folder_id = yandex_resourcemanager_folder.folder.id
  name              = var.k8s-kms-key-name
  description       = "K8s kms key"
  default_algorithm = "AES_256"
}

resource "yandex_kubernetes_cluster" "k8s" {
 folder_id = yandex_resourcemanager_folder.folder.id
 name        = var.k8s-name
 network_id = yandex_vpc_network.network.id
 master {
   zonal {
     zone      = yandex_vpc_subnet.subnet.zone
     subnet_id = yandex_vpc_subnet.subnet.id
   }
   #Disable Public IP in prod
   public_ip = true
   maintenance_policy {
      auto_upgrade = true
      maintenance_window {
        day        = "sunday"
        start_time = "00:00"
        duration   = "3h"
      }
  }
 }
 kms_provider {
    key_id = "${yandex_kms_symmetric_key.k8s-kms-key.id}"
 }
 release_channel = "STABLE"
 service_account_id      = yandex_iam_service_account.k8s-sa.id
 node_service_account_id = yandex_iam_service_account.k8s-nsa.id
   depends_on = [
     yandex_resourcemanager_folder_iam_binding.editor,
     yandex_resourcemanager_folder_iam_binding.images-puller
   ]
}

resource "yandex_kubernetes_node_group" "k8s-nodes" {
  cluster_id = yandex_kubernetes_cluster.k8s.id
  name        = var.k8s-nodes.name
  version     = var.k8s-nodes.version
  instance_template {
    platform_id = var.k8s-nodes.platform
    resources {
      memory = var.k8s-nodes.memory
      cores  = var.k8s-nodes.cpu
    }
    boot_disk {
      type = "network-ssd"
      size = var.k8s-nodes.disk
    }
  }
  scale_policy {
    auto_scale {
      min     = var.k8s-nodes.min
      max     = var.k8s-nodes.max
      initial = var.k8s-nodes.initial
    }
  }
  maintenance_policy {
  auto_upgrade = true
  auto_repair  = true
  maintenance_window {
    day        = "sunday"
    start_time = "00:00"
    duration   = "3h"
    }
  }
}
