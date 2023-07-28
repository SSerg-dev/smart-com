variable "cloud_id" {
    description = "Cloud ID"
}

variable "deploy-sa-authkey" {
    description = "YC Authorized key"
}

variable "folder" {
    description = "YC Folder name"
}

variable "dns-zone" {
    description = "Dns zone name"
}

variable "folder_net" {
    description = "YC Network folder name"
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

variable "natgw-name" {
    description = "Nat Gateway name"
}

variable "rt-name" {
    description = "Routing table name"
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

variable "k8s-cluster-cidr" {
    description = "K8s cluster CIDR"
}

variable "k8s-services-cidr" {
    description = "K8s CIDR services"
}

variable "k8s-version" {
    description = "K8s version"
}

variable "k8s-nodes" {
    description = "K8s node group"
    type = object({
        name = string
        platform = string
        cpu = number
        cpu_fraction = number
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

variable "bucket-size" {
    description = "Size for Storage"
}

variable "bucket-kms-name" {
    description = "Kms key for bucket encryption"
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
        ssh-key = string
    })
}

variable "vault-sa-name" {
    description = "Service account Vault"
}

variable "vault-kms-name" {
    description = "KMS for Vault"
}

variable "k8s-vault-namespace" {
    description = "Namespace for Config services"
}

variable "airflow-sa-name" {
    description = "Service account Airflow"
}

variable "airflow-image" {
    description = "Registry with Airflow image"
}

variable "airflow-tag" {
    description = "Airflow image tag"
}

variable "airflow-dags-pv" {
    description = "Airflow volume name in k8s"
}

variable "airflow-dags-pvc" {
    description = "Airflow volume claim name in k8s"
}

variable "function-sa-name" {
    description = "Service account for function"
}

variable "function-start-name" {
    description = "Function start name"
}

variable "function-stop-name" {
    description = "Function stop name"
}

variable "kafka" {
    description = "Kafka cluster"
    type = object({
        name = string
        preset_id = string
        version = string
        disk = number
        user = string
        pass = string
    })
}

variable "kafka-proxy" {
    description = "Kafka Proxy VM"
    type = object({
        name = string
        platform_id = string
        cpu = number
        cpu_fraction = number
        memory = number
        disk = number
        sa-name = string
        image = string
        user-data = string
        ssh-keys = string
    })
}

variable "deploy-sa-name" {
    description = "Deployment service account"
}