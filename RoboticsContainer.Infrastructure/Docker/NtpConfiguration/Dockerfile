# Use the cturra/ntp image as the base image
FROM cturra/ntp:latest

# Expose the NTP port (123/udp)
EXPOSE 123/udp

# Start the NTP server
CMD ["ntpd", "-n", "-g", "-u", "ntp:ntp"]