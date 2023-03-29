cloud_id = ""
token = "IAM token"
folder = "folder name"
zone = "zone name"

network-name = "network"
subnet-name = "subnet-1"
subnet-cidr = ["172.20.3.0/24"]

registry-name = "registry"

k8s-sa-name = "k8s-sa"
k8s-nsa-name = "k8s-nsa"
k8s-kms-key-name = "k8s-key"
k8s-name = "k8s"
k8s-nodes = {
    name = "k8s-nodes"
    version = "1.20"
    platform = "standard-v3"
    cpu = 16
    memory = 48
    disk = 512
    min = 3
    max = 4
    initial = 3
}

bucket-name = "bucket"
dataproc-sa-name = "dataproc-sa"
dataproc = {
    name = "dataproc"
    preset_id_master = "s2.large"
    preset_id_data = "s2.large"
    preset_id_compute = "s2.large"
    disk_master = 2048
    disk_data = 2048
    disk_compute = 2048
    compute_min = 1
    compute_max = 3
}

vault-sa-name = "vault-sa"
vault-kms-key-name = "vault-kms"
vault = {
    name = "vault"
    platform_id = "standard-v3"
    cpu = 4
    memory = 4
}

airflow-sa-name = "airflow-sa"
airflowRepoPath = "Git with Dags"