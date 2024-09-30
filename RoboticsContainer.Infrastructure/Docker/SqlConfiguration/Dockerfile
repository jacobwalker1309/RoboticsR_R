# Use the official MySQL image as a base
FROM mysql:latest

# Set environment variables for MySQL
ENV MYSQL_ROOT_PASSWORD=Admin@123
ENV MYSQL_DATABASE=TestDB

# Copy SQL scripts to MySQL init directory
ADD ./sql-scripts/ /docker-entrypoint-initdb.d/

# Ensure that the scripts are executable
RUN chmod -R 755 /docker-entrypoint-initdb.d/

# Expose MySQL port
EXPOSE 3306
