
resource "helm_release" "superset" {
  namespace        = "superset"
  create_namespace = true
  name             = "superset"
  repository       = "https://apache.github.io/superset"
  chart            = "superset"
  wait             = true
  timeout    = 600
  depends_on = [
    yandex_kubernetes_node_group.k8s-nodes
  ]
  values = [
    templatefile("./superset-values.yaml", {
        subnetId = yandex_vpc_subnet.subnet.id
        })
  ]
}