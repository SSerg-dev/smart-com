cloud_id = "b1g0ngqm213bgkm0uqtt"
folder_net = "smartcom-env-rg"
zone = "ru-central1-b"
network-name = "smartcom-env-network"

folder = "jupiter-app-dev-rg"

subnet-name = "jupiter-app-dev-subnet"
subnet-cidr = ["172.20.1.0/24"]
natgw-name = "jupiter-app-dev-natgw"
rt-name = "jupiter-app-dev-rt-2"

registry-name = "jupiter-app-dev-registry"

k8s-sa-name = "jupiter-app-dev-k8s-sa"
k8s-nsa-name = "jupiter-app-dev-k8sn-sa"
k8s-kms-key-name = "jupiter-app-dev-k8s-kms"
k8s-name = "jupiter-app-dev-k8s"
k8s-version = "1.23"
k8s-cluster-cidr = "10.111.0.0/16"
k8s-services-cidr = "10.95.0.0/16"
k8s-vault-namespace = "vault"
k8s-nodes = {
    name = "jupiter-app-dev-k8s-nodes"
    platform = "standard-v3"
    cpu = 2
    cpu_fraction = 20
    memory = 8
    disk = 64
    min = 1
    max = 3
    initial = 1
}

vault-sa-name = "jupiter-app-dev-vault-sa"
vault-kms-name = "jupiter-app-dev-vault-kms"

bucket-name = "jupiter-app-dev-bucket"
bucket-size = 53687091200
bucket-kms-name = "jupiter-app-dev-bucket-kms"

function-sa-name = "jupiter-app-dev-function-sa"
function-start-name = "jupiter-app-dev-start-function"
function-stop-name = "jupiter-app-dev-stop-function"

dataproc-sa-name = "jupiter-app-dev-dataproc-sa"
dataproc = {
    name = "jupiter-app-dev-dataproc"
    preset_id_master = "s2.small"
    preset_id_data = "s2.small"
    preset_id_compute = "s2.small"
    disk_master = 64
    disk_data = 64
    disk_compute = 64
    compute_min = 1
    compute_max = 3
    ssh-key = "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQDhs734/KlEdJyOl6I2EmzvLDOdTrnrMuj7LTIwQSL5RjG+HEVr6yCBCeWRrNkBU2Lp2IdsN/FXtm4mZjNcezfhyJ+zcZCrgSyKYBpczXd22s54mXdNjjJ9nm1Xzzj9l7FyuZIFVb3Y5kcBqt6+XK3XQf46pEdLZqW3nUcFZPfbBnysKcgfLvQtFsmFSZjolbApH9B+6C+oi4y3Ls/YKwBTE+JkGy/MRaJkT9eyYzmAbQeretwHCsl83RwD90kNtIN9bidgdiA9R1H/V58l0376MUqmZHjjhMLgkezDLfpPM3EK9LSdH8mmt4198iYOkn24OfdoofwB3LGAUsVXAxy3 smartadmin"
}

airflow-sa-name = "jupiter-app-dev-airflow-sa"
airflow-image = "cr.yandex/crp82nk4hju0v8d8r6pt/airflow"
airflow-tag = "airflow-jupiter-dev"
airflow-dags-pv = "airflow-dags"
airflow-dags-pvc = "airflow-dags-pvc"

kafka = {
    name = "jupiter-app-dev-kafka"
    preset_id = "b3-c1-m4"
    version = "3.0"
    disk = 32
    user = "jupiter-user"
    pass = "pass1234"
}

kafka-proxy = {
    name = "jupiter-app-dev-kafka-proxy-vm"
    platform_id = "standard-v3"
    cpu = 2
    cpu_fraction = 20
    memory = 2
    disk = 30
    sa-name = "jupiter-app-dev-kafka-proxy-sa"
    image = "cr.yandex/crp82nk4hju0v8d8r6pt/airflow:yandex-kafka-rest"
}