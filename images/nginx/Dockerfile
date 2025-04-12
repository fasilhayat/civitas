# Use official Nginx image as a base
FROM nginx:alpine

# Copy your web files to the default Nginx webserver folder
COPY ./static/ /usr/share/nginx/html

# Expose ports
EXPOSE 8080
EXPOSE 9443

# Start Nginx when the container runs
CMD ["nginx", "-g", "daemon off;"]
