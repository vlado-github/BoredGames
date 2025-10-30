FROM nginx:stable-alpine

RUN apk add --no-cache git gcc libc-dev make openssl-dev pcre-dev zlib-dev linux-headers curl \
    && cd /tmp \
    && git clone --recurse-submodules -j8 https://github.com/google/ngx_brotli \
    && cd ngx_brotli/deps/brotli \
    && mkdir out && cd out \
    && cmake -DCMAKE_BUILD_TYPE=Release -DBUILD_SHARED_LIBS=OFF .. \
    && cmake --build . --config Release --target brotlienc \
    && cd /tmp \
    && NGINX_VERSION=$(nginx -v 2>&1 | cut -d/ -f2) \
    && curl -fSL http://nginx.org/download/nginx-${NGINX_VERSION}.tar.gz -o nginx.tar.gz \
    && tar -xzf nginx.tar.gz \
    && cd nginx-${NGINX_VERSION} \
    && ./configure --with-compat --add-dynamic-module=/tmp/ngx_brotli $(nginx -V 2>&1 | grep -o 'configure arguments:.*' | sed 's/configure arguments://') \
    && make modules \
    && cp objs/*.so /etc/nginx/modules/ \
    && cd / \
    && rm -rf /tmp/* \
    && apk del git gcc libc-dev make

EXPOSE 80 443

CMD ["nginx", "-g", "daemon off;"]