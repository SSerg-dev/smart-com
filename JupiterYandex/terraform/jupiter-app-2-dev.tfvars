organization_id = "bpfuurgn7jf9lvpss6co"
cloud_id = "b1g0ngqm213bgkm0uqtt"
folder_net = "smartcom-env-rg"
zone = "ru-central1-b"
network-name = "smartcom-env-network"

folder = "jupiter-app-2-dev-rg"
dns-zone = "jupiter-app-2-dev"

subnet-name = "jupiter-app-2-dev-subnet"
subnet-cidr = ["172.20.2.0/24"]
natgw-name = "jupiter-app-2-dev-natgw"
rt-name = "jupiter-app-2-dev-rt"

registry-name = "jupiter-app-2-dev-registry"

k8s-sa-name = "jupiter-app-2-dev-k8s-sa"
k8s-nsa-name = "jupiter-app-2-dev-k8sn-sa"
k8s-kms-key-name = "jupiter-app-2-dev-k8s-kms"
k8s-name = "jupiter-app-2-dev-k8s"
k8s-version = "1.27"
k8s-cluster-cidr = "10.112.0.0/16"
k8s-services-cidr = "10.96.0.0/16"
k8s-vault-namespace = "vault"
k8s-nodes = {
    name = "jupiter-app-2-dev-k8s-nodes"
    platform = "standard-v3"
    cpu = 2
    cpu_fraction = 20
    memory = 8
    disk = 64
    min = 1
    max = 3
    initial = 1
}

vault-sa-name = "jupiter-app-2-dev-vault-sa"
vault-kms-name = "jupiter-app-2-dev-vault-kms"

bucket-name = "jupiter-app-2-dev-bucket"
bucket-size = 53687091200
bucket-kms-name = "jupiter-app-2-dev-bucket-kms"

function-sa-name = "jupiter-app-2-dev-function-sa"
function-start-name = "jupiter-app-2-dev-start-function"
function-stop-name = "jupiter-app-2-dev-stop-function"

dataproc-sa-name = "jupiter-app-2-dev-dataproc-sa"
dataproc = {
    name = "jupiter-app-2-dev-dataproc"
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

airflow-sa-name = "jupiter-app-2-dev-airflow-sa"
airflow-image = "cr.yandex/crphfbb3tge0u19a9s58/airflow"
airflow-tag = "airflow-jupiter-dev"
airflow-dags-pv = "airflow-dags"
airflow-dags-pvc = "airflow-dags-pvc"

kafka = {
    name = "jupiter-app-2-dev-kafka"
    preset_id = "b3-c1-m4"
    version = "3.0"
    disk = 32
    user = "jupiter-user"
    pass = "pass1234"
}

kafka-proxy = {
    name = "jupiter-app-2-dev-kafka-proxy-vm"
    platform_id = "standard-v3"
    cpu = 2
    cpu_fraction = 20
    memory = 2
    disk = 30
    sa-name = "jupiter-app-2-dev-kafka-proxy-sa"
    image = "cr.yandex/crphfbb3tge0u19a9s58/airflow:yandex-kafka-rest"
    user-data = <<-EOT
#cloud-config
datasource:
 Ec2:
  strict_id: false
ssh_pwauth: no
users:
- name: smartadmin
  sudo: ALL=(ALL) NOPASSWD:ALL
  shell: /bin/bash
  ssh_authorized_keys:
  - ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQDhs734/KlEdJyOl6I2EmzvLDOdTrnrMuj7LTIwQSL5RjG+HEVr6yCBCeWRrNkBU2Lp2IdsN/FXtm4mZjNcezfhyJ+zcZCrgSyKYBpczXd22s54mXdNjjJ9nm1Xzzj9l7FyuZIFVb3Y5kcBqt6+XK3XQf46pEdLZqW3nUcFZPfbBnysKcgfLvQtFsmFSZjolbApH9B+6C+oi4y3Ls/YKwBTE+JkGy/MRaJkT9eyYzmAbQeretwHCsl83RwD90kNtIN9bidgdiA9R1H/V58l0376MUqmZHjjhMLgkezDLfpPM3EK9LSdH8mmt4198iYOkn24OfdoofwB3LGAUsVXAxy3 smartadmin
EOT
    ssh-keys = "smartadmin:ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQDhs734/KlEdJyOl6I2EmzvLDOdTrnrMuj7LTIwQSL5RjG+HEVr6yCBCeWRrNkBU2Lp2IdsN/FXtm4mZjNcezfhyJ+zcZCrgSyKYBpczXd22s54mXdNjjJ9nm1Xzzj9l7FyuZIFVb3Y5kcBqt6+XK3XQf46pEdLZqW3nUcFZPfbBnysKcgfLvQtFsmFSZjolbApH9B+6C+oi4y3Ls/YKwBTE+JkGy/MRaJkT9eyYzmAbQeretwHCsl83RwD90kNtIN9bidgdiA9R1H/V58l0376MUqmZHjjhMLgkezDLfpPM3EK9LSdH8mmt4198iYOkn24OfdoofwB3LGAUsVXAxy3 smartadmin"
}

deploy-sa-name = "jupiter-app-2-dev-deploy-sa"
admins = ["ajeehctfa42f65mi0vnl", "ajeh4495unpnso3pbctt", "ajeh1g5kg4fns2v8un96", "ajejp42omjeiha4liqmi","aje1lft5tu15rloh741t" ]

datasphere-sa-name = "jupiter-app-2-dev-datasphere-sa"