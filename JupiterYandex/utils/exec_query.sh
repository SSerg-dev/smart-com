#!/bin/sh

get_duration()
{
    local start_ts=$1
    local end_ts=$(date +%s%N | cut -b1-13)
    local duration=$((end_ts - start_ts))

    return $duration
}

start_ts=$(date +%s%N | cut -b1-13)

tfifo=$(mktemp -d)/fifo
mkfifo $tfifo
db_params=$(echo $3|base64 -d)

COPY_TO_HDFS_CMD='cat $tfifo | hadoop dfs -put -f - $2'	
if [ ! -z "$6" ] #Attach header
then
  echo "Creating header..."
  echo "$6" | hadoop dfs -put -f - $2
  COPY_TO_HDFS_CMD='cat $tfifo | hadoop dfs -appendToFile - $2'	
fi
	
/opt/mssql-tools18/bin/bcp "$1" queryout $tfifo ""$db_params"" -u -c -t $4 &


pid=$! # get PID of backgrounded bcp process
count=$(ps -p $pid -o pid= |wc -l) # check whether process is still running

echo $pid

if [ $count -eq 0 ] # if process is already terminated, something went wrong
then
    echo "something went wrong with bcp command"
    rm $tfifo
    wait $pid
	ret_code = $?
	get_duration $start_ts
	duration=$?
	
	echo "{\"Schema\":\"$5\",\"EntityName\":\"$(basename $2 .csv)\",\"Result\":false,\"Duration\":\"$duration\"}"
    exit $ret_code
else
    echo "Write body..."
    eval $COPY_TO_HDFS_CMD
	
    wait $pid
	ret_code=$?
    echo "Bcp return code="$ret_code 	
	
    get_duration $start_ts
	duration=$?
	
	if [ $ret_code -eq 0 ];# Check bcp result
	then
		echo "{\"Schema\":\"$5\",\"EntityName\":\"$(basename $2 .csv)\",\"Result\":true,\"Duration\":\"$duration\"}"
	else
		echo "{\"Schema\":\"$5\",\"EntityName\":\"$(basename $2 .csv)\",\"Result\":false,\"Duration\":\"$duration\"}"
	fi
    # exit $ret_code
	
fi


