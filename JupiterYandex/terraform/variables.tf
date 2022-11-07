variable "cloud_id" {
    description = "Cloud ID"
}

variable "token" {
    description = "YC user IAM Token"
    sensitive = true
}

variable "folder" {
    description = "YC Folder name"
}

variable "zone" {
    description = "YC zone"
}

variable "network-name" {
    description = "Network name"
}

variable "subnet-name" {
    description = "Subnet name"
}

variable "subnet-cidr" {
    description = "Subnet v4_cidr_blockse"
}

variable "registry-name" {
    description = "Container registry name"
}

variable "k8s-sa-name" {
    description = "Service account k8s"
}

variable "k8s-nsa-name" {
    description = "Service account k8s for nodes"
}

variable "k8s-kms-key-name" {
    description = "KMS k8s for nodes"
}

variable "k8s-name" {
    description = "K8s name"
}

variable "k8s-nodes" {
    description = "K8s node group"
    type = object({
        name = string
        version = string
        platform = string
        cpu = number
        memory = number
        disk = number
        min = number
        max = number
        initial = number
    })
}

variable "bucket-name" {
    description = "Storage for Dataproc"
}

variable "dataproc-sa-name" {
    description = "Service account Dataproc"
}

variable "dataproc" {
    description = "Dataproc cluster"
    type = object({
        name = string
        preset_id_master = string
        preset_id_data = string
        preset_id_compute = string
        disk_master = number
        disk_data = number
        disk_compute = number
        compute_min = number
        compute_max = number
    })
}

variable "vault-sa-name" {
    description = "Service account Vault"
}

variable "vault-kms-key-name" {
    description = "KMS for Vault"
}

variable "vault" {
    description = "Hashicorp Vault VM"
    type = object({
        name = string
        platform_id = string
        cpu = number
        memory = number
    })
}

variable "airflow-sa-name" {
    description = "Service account Airflow"
}

variable "airflowRepoPath" {
    description = "Repository with Airflow dags"
}


