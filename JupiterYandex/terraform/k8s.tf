resource "yandex_iam_service_account" "k8s-sa" {
 name        = var.k8s-sa-name
 description = "Service account k8s"
 folder_id = yandex_resourcemanager_folder.folder.id
}

resource "yandex_resourcemanager_folder_iam_binding" "k8s-clusters-agent" {
 folder_id = yandex_resourcemanager_folder.folder.id
 role      = "k8s.clusters.agent"
 members   = [
   "serviceAccount:${yandex_iam_service_account.k8s-sa.id}"
 ]
}

resource "yandex_resourcemanager_folder_iam_binding" "load-balancer-admin" {
 folder_id = yandex_resourcemanager_folder.folder.id
 role      = "load-balancer.admin"
 members   = [
   "serviceAccount:${yandex_iam_service_account.k8s-sa.id}"
 ]
}

resource "yandex_resourcemanager_folder_iam_binding" "load-balancer-admin2" {
 folder_id = data.yandex_resourcemanager_folder.folder_net.id
 role      = "load-balancer.admin"
 members   = [
   "serviceAccount:${yandex_iam_service_account.k8s-sa.id}"
 ]
}

resource "yandex_resourcemanager_folder_iam_binding" "vpc-privateAdmin" {
 folder_id = data.yandex_resourcemanager_folder.folder_net.id
 role      = "vpc.privateAdmin"
 members   = [
   "serviceAccount:${yandex_iam_service_account.k8s-sa.id}"
 ]
}

resource "yandex_resourcemanager_folder_iam_binding" "vpc-bridgeAdmin" {
 folder_id = data.yandex_resourcemanager_folder.folder_net.id
 role      = "vpc.bridgeAdmin"
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
  depends_on = [
    yandex_resourcemanager_folder_iam_binding.k8s-clusters-agent,
    yandex_resourcemanager_folder_iam_binding.images-puller,
    yandex_resourcemanager_folder_iam_binding.vpc-privateAdmin,
    yandex_resourcemanager_folder_iam_binding.vpc-bridgeAdmin,
    yandex_resourcemanager_folder_iam_binding.load-balancer-admin,
    yandex_resourcemanager_folder_iam_binding.load-balancer-admin2
  ]
  folder_id = yandex_resourcemanager_folder.folder.id
  name        = var.k8s-name
  service_account_id      = yandex_iam_service_account.k8s-sa.id
  node_service_account_id = yandex_iam_service_account.k8s-nsa.id
  network_id = data.yandex_vpc_network.network.id
  cluster_ipv4_range = var.k8s-cluster-cidr
  service_ipv4_range = var.k8s-services-cidr
  release_channel = "STABLE"
  master {
    zonal {
      zone      = yandex_vpc_subnet.subnet.zone
      subnet_id = yandex_vpc_subnet.subnet.id
    }
    public_ip = false
    version = var.k8s-version
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
}

resource "yandex_kubernetes_node_group" "k8s-nodes" {
  cluster_id = yandex_kubernetes_cluster.k8s.id
  name        = var.k8s-nodes.name
  instance_template {
    platform_id = var.k8s-nodes.platform
    resources {
      memory = var.k8s-nodes.memory
      cores  = var.k8s-nodes.cpu
      core_fraction = var.k8s-nodes.cpu_fraction
    }
    boot_disk {
      type = "network-ssd"
      size = var.k8s-nodes.disk
    }
    scheduling_policy {
      preemptible = true
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