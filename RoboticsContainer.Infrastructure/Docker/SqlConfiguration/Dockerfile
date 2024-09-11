# Use the official SQL Server 2022 image as a base
FROM mcr.microsoft.com/mssql/server:2022-latest

# Set environment variables for SQL Server
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=Admin@123
ENV MSSQL_DATA_DIR=/var/opt/mssql/data
ENV MSSQL_LOG_DIR=/var/opt/mssql/log
ENV MSSQL_SECRETS_DIR=/var/opt/mssql/secrets
ENV MSSQL_PID=Developer
ENV MSSQL_TCP_PORT=1433

# Install necessary tools and GPG key
USER root
RUN apt-get update && \
    apt-get install -y --no-install-recommends apt-utils gnupg2 wget && \
    wget -qO- https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    wget -qO /etc/apt/sources.list.d/mssql-release.list https://packages.microsoft.com/config/ubuntu/22.04/prod.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y msodbcsql18 mssql-tools18

# Add mssql-tools18 to the PATH
ENV PATH="${PATH}:/opt/mssql-tools18/bin"

# Clean up to reduce image size
RUN apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Expose SQL Server port
EXPOSE 1433

# Use ADD to copy the entire sql-scripts directory to the container
ADD ./sql-scripts/ /sql-scripts/

# Ensure that the script and directory are executable and readable
RUN chmod -R 755 /sql-scripts

# Start SQL Server and run the initialization script
CMD /bin/bash -c "/opt/mssql/bin/sqlservr & sleep 40 && sqlcmd -S localhost -U sa -P 'Admin@123' -d master -i /sql-scripts/init.sql -C && wait"
