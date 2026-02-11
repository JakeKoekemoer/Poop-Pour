const { createApp } = Vue;

createApp({
    data() {
        return {
            config: null,  // Will be loaded from swagger-config.json
            copied: false,
            endpointCount: 0,
            schemaCount: 0,
            availableMethods: [],
            methodFilters: [],
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
    
    async mounted() {
        await this.loadConfig();
        this.initSwagger();
    },
    
    methods: {
        async loadConfig() {
            try {
                const response = await fetch('/swagger/swagger-config.json');
                if (!response.ok) {
                    throw new Error(`Failed to load config: ${response.status}`);
                }
                this.config = await response.json();
                
                // Update page title
                document.getElementById('page-title').textContent = this.config.title;
                
                // Initialize available methods and filters from config
                this.availableMethods = this.config.methods.available;
                this.methodFilters = [...this.config.methods.available];
            } catch (error) {
                console.error('Failed to load config:', error);
                // Fallback to default config
                this.config = this.getDefaultConfig();
                document.getElementById('page-title').textContent = this.config.title;
                this.availableMethods = this.config.methods.available;
                this.methodFilters = [...this.config.methods.available];
            }
        },
        
        getDefaultConfig() {
            return {
                title: "API Documentation",
                header: { logoEmoji: "📄", appName: "API", appSubtitle: "Documentation", version: "v1.0" },
                navigation: {
                    homeLabel: "Home", homeUrl: "/",
                    openApiSpecLabel: "OpenAPI Spec", openApiSpecUrl: "/swagger/v1/swagger.json",
                    copySpecLabel: "Copy Spec", copySpecLabelCopied: "Copied!",
                    quickLoginLabel: "Quick Login", quickLoginLabelLoading: "Logging in..."
                },
                quickActions: { endpointsLabel: "{count} endpoints", schemasLabel: "{count} schemas" },
                footer: {
                    copyright: "© 2026", privacyPolicyLabel: "Privacy", privacyPolicyUrl: "#",
                    termsLabel: "Terms", termsUrl: "#",
                    poweredByLabel: "Powered by Swagger", poweredByUrl: "https://swagger.io"
                },
                authentication: {
                    defaultUsername: "admin", defaultPassword: "admin",
                    loginSuccessMessage: "Logged in successfully!",
                    loginErrorMessage: "Login failed: {error}",
                    swaggerNotReadyMessage: "Swagger UI not ready",
                    authenticatedLabel: "Authenticated"
                },
                toast: { specCopiedMessage: "Copied!", specCopyFailedMessage: "Failed to copy" },
                swagger: { specUrl: "/swagger/v1/swagger.json", domId: "#swagger-ui" },
                methods: { available: ["GET", "POST", "PUT", "DELETE"] }
            };
        },
        
        formatLabel(template, count) {
            if (!template) return '';
            const parts = template.split('{count}');
            if (parts.length === 2) {
                return parts[0] + '<span class="text-slate-200">' + count + '</span>' + parts[1];
            }
            return template;
        },
        initSwagger() {
            const ui = SwaggerUIBundle({
                url: this.config.swagger.specUrl,
                dom_id: this.config.swagger.domId,
                deepLinking: this.config.swagger.deepLinking,
                presets: [SwaggerUIBundle.presets.apis, SwaggerUIStandalonePreset],
                plugins: [SwaggerUIBundle.plugins.DownloadUrl],
                layout: "BaseLayout",
                tryItOutEnabled: this.config.swagger.tryItOutEnabled,
                filter: this.config.swagger.filter,
                displayRequestDuration: this.config.swagger.displayRequestDuration,
                defaultModelsExpandDepth: this.config.swagger.defaultModelsExpandDepth,
                docExpansion: this.config.swagger.docExpansion,
                persistAuthorization: this.config.swagger.persistAuthorization,
                onComplete: () => {
                    this.countEndpoints();
                    this.countSchemas();
                }
            });
            window.ui = ui;
        },
        
        async copySpec() {
            try {
                const response = await fetch(this.config.navigation.openApiSpecUrl);
                const spec = await response.text();
                await navigator.clipboard.writeText(spec);
                this.copied = true;
                this.showToast(this.config.toast.specCopiedMessage, 'success');
                setTimeout(() => this.copied = false, 2000);
            } catch (err) {
                this.showToast(this.config.toast.specCopyFailedMessage, 'error');
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
                
                // Call the login endpoint with credentials from config
                const response = await fetch(`${baseUrl}/api/authentication/login`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        username: this.config.authentication.defaultUsername,
                        password: this.config.authentication.defaultPassword
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
                    // Use Swagger UI's preauthorizeApiKey method with correct security scheme name
                    window.ui.preauthorizeApiKey('Bearer', token);
                    
                    this.showToast(this.config.authentication.loginSuccessMessage, 'success');
                    
                    // Update the authorize button appearance
                    setTimeout(() => {
                        this.updateAuthorizeButton(true);
                    }, 500);
                } else {
                    this.showToast(this.config.authentication.swaggerNotReadyMessage, 'error');
                }
            } catch (err) {
                console.error('Quick login error:', err);
                const errorMessage = this.config.authentication.loginErrorMessage.replace('{error}', err.message);
                this.showToast(errorMessage, 'error');
            } finally {
                this.isLoggingIn = false;
            }
        },
        
        updateAuthorizeButton(isAuthenticated) {
            const authBtn = document.querySelector('.btn.authorize');
            if (authBtn && isAuthenticated) {
                // Manually add the locked class to change appearance
                authBtn.classList.remove('unlocked');
                authBtn.classList.add('locked');
                
                // Change button text to show authenticated state
                const btnText = authBtn.querySelector('span');
                if (btnText) {
                    btnText.textContent = this.config.authentication.authenticatedLabel;
                }
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
