# Use the official Redis image as a base
FROM redis:latest

# Optional: Copy a custom Redis configuration file
# COPY ./redis.conf /usr/local/etc/redis/redis.conf

# Expose the Redis port
EXPOSE 6379

# Command to run Redis with a custom configuration file
# CMD ["redis-server", "/usr/local/etc/redis/redis.conf"]

# If no custom configuration is needed, you can use the default entrypoint
CMD ["redis-server"]
