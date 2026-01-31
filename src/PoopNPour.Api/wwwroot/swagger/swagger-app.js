const { createApp } = Vue;

createApp({
    data() {
        return {
            copied: false,
            endpointCount: 0,
            schemaCount: 0,
            availableMethods: ['GET', 'POST', 'PUT', 'DELETE'],
            methodFilters: ['GET', 'POST', 'PUT', 'DELETE'],
            methodColors: {
                GET: { active: 'bg-blue-500 text-white', inactive: 'bg-slate-700 text-slate-400 hover:bg-slate-600' },
                POST: { active: 'bg-green-500 text-white', inactive: 'bg-slate-700 text-slate-400 hover:bg-slate-600' },
                PUT: { active: 'bg-amber-500 text-white', inactive: 'bg-slate-700 text-slate-400 hover:bg-slate-600' },
                DELETE: { active: 'bg-red-500 text-white', inactive: 'bg-slate-700 text-slate-400 hover:bg-slate-600' },
            },
            toast: { show: false, message: '', type: 'success' },
            isLoggingIn: false
        };
    },
    
    mounted() {
        this.initSwagger();
    },
    
    methods: {
        initSwagger() {
            const ui = SwaggerUIBundle({
                url: "/swagger/v1/swagger.json",
                dom_id: '#swagger-ui',
                deepLinking: true,
                presets: [SwaggerUIBundle.presets.apis, SwaggerUIStandalonePreset],
                plugins: [SwaggerUIBundle.plugins.DownloadUrl],
                layout: "BaseLayout",
                tryItOutEnabled: true,
                filter: true,
                displayRequestDuration: true,
                defaultModelsExpandDepth: 2,
                docExpansion: "list",
                persistAuthorization: true,
                onComplete: () => {
                    this.countEndpoints();
                    this.countSchemas();
                }
            });
            window.ui = ui;
        },
        
        async copySpec() {
            try {
                const response = await fetch('/swagger/v1/swagger.json');
                const spec = await response.text();
                await navigator.clipboard.writeText(spec);
                this.copied = true;
                this.showToast('OpenAPI spec copied to clipboard!', 'success');
                setTimeout(() => this.copied = false, 2000);
            } catch (err) {
                this.showToast('Failed to copy spec', 'error');
            }
        },
        
        authorize() {
            const authBtn = document.querySelector('.btn.authorize');
            if (authBtn) authBtn.click();
        },
        
        async quickLogin() {
            this.isLoggingIn = true;
            try {
                // Get the base URL from the current window location
                const baseUrl = window.location.origin;
                
                // Call the login endpoint with default admin credentials
                const response = await fetch(`${baseUrl}/api/auth/login`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        username: 'admin@poopnpour.com',
                        password: 'Admin@123'
                    })
                });

                if (!response.ok) {
                    const errorText = await response.text();
                    console.error('Login response error:', errorText);
                    throw new Error('Login failed');
                }

                const data = await response.json();
                const token = data.token;

                if (!token) {
                    throw new Error('No token in response');
                }

                // Set the token in Swagger UI's authorization
                if (window.ui) {
                    // Use Swagger UI's preauthorizeApiKey method
                    window.ui.preauthorizeApiKey('Bearer', token);
                    this.showToast('✅ Logged in successfully! Token applied.', 'success');
                } else {
                    this.showToast('⚠️ Swagger UI not ready', 'error');
                }
            } catch (err) {
                console.error('Quick login error:', err);
                this.showToast('❌ Login failed: ' + err.message, 'error');
            } finally {
                this.isLoggingIn = false;
            }
        },
        
        toggleMethodFilter(method) {
            const index = this.methodFilters.indexOf(method);
            if (index > -1) {
                this.methodFilters.splice(index, 1);
            } else {
                this.methodFilters.push(method);
            }
            this.applyMethodFilters();
        },
        
        applyMethodFilters() {
            document.querySelectorAll('.opblock').forEach(block => {
                const method = block.classList.contains('opblock-get') ? 'GET' :
                              block.classList.contains('opblock-post') ? 'POST' :
                              block.classList.contains('opblock-put') ? 'PUT' :
                              block.classList.contains('opblock-delete') ? 'DELETE' : null;
                block.style.display = this.methodFilters.includes(method) ? '' : 'none';
            });
        },
        
        countEndpoints() {
            setTimeout(() => {
                this.endpointCount = document.querySelectorAll('.opblock').length;
            }, 500);
        },
        
        countSchemas() {
            setTimeout(() => {
                this.schemaCount = document.querySelectorAll('.model-container').length;
            }, 500);
        },
        
        showToast(message, type = 'success') {
            this.toast = { show: true, message, type };
            setTimeout(() => this.toast.show = false, 3000);
        }
    }
}).mount('#app');
