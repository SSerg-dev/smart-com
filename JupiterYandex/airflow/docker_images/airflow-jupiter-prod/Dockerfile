FROM apache/airflow:2.4.3-python3.10

USER root
RUN apt-get update
RUN apt-get -y install gnupg2
RUN apt-get -y install curl

# Build MSSQL ODBC driver
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
RUN curl https://packages.microsoft.com/config/debian/10/prod.list > /etc/apt/sources.list.d/mssql-release.list
RUN apt-get update
RUN ACCEPT_EULA=Y apt-get install -y --allow-unauthenticated msodbcsql18
RUN ACCEPT_EULA=Y apt-get install -y --allow-unauthenticated mssql-tools18
RUN echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bash_profile
RUN echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bashrc
RUN apt-get install -y unixodbc-dev
RUN apt-get install -y g++
RUN apt-get -y install libcsv3
RUN apt-get -y install procps 

#Build hadoop with custom config
RUN DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends default-jdk default-jre 

ENV JAVA_HOME=/usr/lib/jvm/java-11-openjdk-amd64/

RUN curl -O https://dist.apache.org/repos/dist/release/hadoop/common/KEYS

RUN gpg --import KEYS

ENV HADOOP_VERSION 3.2.3
ENV HADOOP_URL https://www.apache.org/dist/hadoop/common/hadoop-$HADOOP_VERSION/hadoop-$HADOOP_VERSION.tar.gz

RUN set -x \
    && curl -fSL "$HADOOP_URL" -o /tmp/hadoop.tar.gz \
    && curl -fSL "$HADOOP_URL.asc" -o /tmp/hadoop.tar.gz.asc \
    && gpg --verify /tmp/hadoop.tar.gz.asc \
    && tar -xvf /tmp/hadoop.tar.gz -C /opt/ \
    && rm /tmp/hadoop.tar.gz*

RUN ln -s /opt/hadoop-$HADOOP_VERSION/etc/hadoop /etc/hadoop

RUN mkdir /opt/hadoop-$HADOOP_VERSION/logs

RUN mkdir /hadoop-data

ENV HADOOP_HOME=/opt/hadoop-$HADOOP_VERSION
ENV HADOOP_CONF_DIR=/etc/hadoop
ENV MULTIHOMED_NETWORK=1
ENV USER=root
ENV PATH $HADOOP_HOME/bin/:$PATH	  
COPY core-site.xml /etc/hadoop/core-site.xml
COPY hdfs-site.xml /etc/hadoop/hdfs-site.xml

#Copy auxilary scripts
COPY /utils/. /utils

#Install postgresql client
RUN apt-get -y install postgresql-client

#Install misc utilities
RUN apt-get -y install telnet nano iputils-ping nmap traceroute wget

#Build bcp_import utility
RUN apt-get -y install build-essential libcsv-dev uuid-dev libkrb5-dev
COPY /bcp_import_src/. /utils
WORKDIR /utils/
RUN gcc bcp_import.c -I /opt/microsoft/msodbcsql18/include -I /usr/include/uuid -o bcp_import -lodbc -lmsodbcsql-18 -lcsv -luuid

RUN mkdir -p /hadoop/tmp/mapred/local
RUN chmod -R 777 /hadoop/tmp/mapred

#Install s3cmd
RUN apt-get -y install s3cmd

# Download and extract azcopy
RUN wget -O downloadazcopy.tar.gz https://aka.ms/downloadazcopy-v10-linux && \
    tar -xf downloadazcopy.tar.gz --strip-components=1 && \
    rm -f /usr/bin/azcopy && \
    cp ./azcopy /usr/bin/ && \
    chmod 755 /usr/bin/azcopy && \
    rm -f ./downloadazcopy.tar.gz && rm -f ./azcopy

#Install python modules
USER airflow
COPY requirements.txt requirements.txt
RUN pip3 install -r requirements.txt

#Setup Airflow secret backend
ENV AIRFLOW__SECRETS__BACKEND="airflow.providers.hashicorp.secrets.vault.VaultBackend"
ENV AIRFLOW__SECRETS__BACKEND_KWARGS='{"connections_path": "connections", "variables_path": "variables", "config_path": null, "url": "http://vault-yckms-k8s.airflow:8200", "auth_type": "kubernetes", "kubernetes_role":"airflow","kv_engine_version": 2}'

ENV AIRFLOW__API__AUTH_BACKEND="airflow.api.auth.backend.basic_auth"

#K8s settings
ENV LD_LIBRARY_PATH /usr/lib64:$LD_LIBRARY_PATH	
ENV AIRFLOW__KUBERNETES__WORKER_PODS_CREATION_BATCH_SIZE=10
#ENV AIRFLOW__KUBERNETES__DELETE_WORKER_PODS=False
ENV AIRFLOW__CORE__DAGBAG_IMPORT_TIMEOUT=120
ENV AIRFLOW__CORE__DAG_FILE_PROCESSOR_TIMEOUT=240


#ENV AIRFLOW__SCHEDULER__SCHEDULE_AFTER_TASK_EXECUTION=False
ENV AIRFLOW__CORE__PARALLELISM=250
RUN mkdir /tmp/hadoop/
RUN mkdir /tmp/hadoop/mapred

#Email settings 
ENV AIRFLOW__EMAIL__EMAIL_BACKEND="airflow.utils.email.send_email_smtp"
ENV AIRFLOW__SMTP__SMTP_HOST="smtp.yandex.ru"
ENV AIRFLOW__SMTP__SMTP_SSL=False
ENV AIRFLOW__SMTP__SMTP_PORT=587
ENV AIRFLOW__SMTP__SMTP_PASSWORD=
ENV AIRFLOW__SMTP__SMTP_USER="infrastructurealerts@smartcom.support"
ENV AIRFLOW__SMTP__SMTP_STARTTLS=True
ENV AIRFLOW__SMTP__SMTP_MAIL_FROM="infrastructurealerts@smartcom.support"

#ADD /dags /opt/airflow/dags



