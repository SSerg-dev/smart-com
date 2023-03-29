terraform {
  required_providers {
    yandex = {
      source = "yandex-cloud/yandex"
      version = "0.78.2"
    }
  }

}

provider "yandex" {
  cloud_id  = var.cloud_id
  token     = var.token
}

provider "helm" {
  kubernetes {
    host                   = yandex_kubernetes_cluster.k8s.master[0].external_v4_endpoint
    cluster_ca_certificate = yandex_kubernetes_cluster.k8s.master[0].cluster_ca_certificate
    exec {
      api_version = "client.authentication.k8s.io/v1beta1"
      args        = ["k8s", "create-token"]
      command     = "yc"
    }
  }
}

resource "yandex_resourcemanager_folder" "folder" {
  cloud_id    = var.cloud_id
  name        = var.folder
}