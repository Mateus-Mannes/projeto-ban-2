export PGPASSWORD='postgres'
psql -U postgres -h db -c "CREATE DATABASE gv;"
psql -U postgres -h db -d gv -a -f  create_db.sql