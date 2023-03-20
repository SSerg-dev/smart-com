#!/bin/sh

get_duration()
{
    local start_ts=$1
    local end_ts=$(date +%s%N | cut -b1-13)
    local duration=$((end_ts - start_ts))

    return $duration
}

start_ts=$(date +%s%N | cut -b1-13)

db_params=$(echo $3|base64 -d)

export PGPASSWORD=$(echo $4|base64 -d)
# COPY_TO_HDFS_CMD='cat $tfifo | hadoop dfs -put -f - $2'	
# $1 - query, $2 - hdfs path, $3 - db parameters, $4 - separator,$5 - schema
echo $5
	
$db_params -c "\copy ($1) to STDOUT with csv header delimiter E'$5'"|hadoop dfs -put -f - $2
ret_code=$?
#eval $COPY_TO_HDFS_CMD

echo "Postgres copy return code="$ret_code 	
	
get_duration $start_ts
duration=$?
	
if [ $ret_code -eq 0 ];# Check bcp result
then
   echo "{\"Schema\":\"$6\",\"EntityName\":\"$(basename $2 .csv)\",\"Result\":true,\"Duration\":\"$duration\"}"
else
   echo "{\"Schema\":\"$6\",\"EntityName\":\"$(basename $2 .csv)\",\"Result\":false,\"Duration\":\"$duration\"}"
   exit $ret_code
fi
