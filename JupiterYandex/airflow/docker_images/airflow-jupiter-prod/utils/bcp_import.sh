#!/bin/sh
if ! hadoop dfs -ls $1; then
    echo Error! Source file not found.
	exit 1
fi

db_params=$(echo $2|base64 -d)

hadoop dfs -cat $1|/utils/bcp_import -s "$db_params" ""$3"" "$4"