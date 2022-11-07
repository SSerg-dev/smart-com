all:
	gcc bcp_import.c -I /opt/microsoft/msodbcsql18/include -I /usr/include/uuid -o bcp_import -lodbc -lmsodbcsql-18 -lcsv -luuid

clean:
	rm -f $(OBJS) $(OUT)