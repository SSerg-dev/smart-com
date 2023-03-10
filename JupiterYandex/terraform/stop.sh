#!/bin/bash
yc managed-kubernetes cluster stop --id $k8s_id --cloud-id $cloud_id --folder-id $folder_id
yc managed-kafka cluster stop --id $kafka_id --cloud-id $cloud_id --folder-id $folder_id
yc dataproc cluster stop --id $dataproc_id --cloud-id $cloud_id --folder-id $folder_id