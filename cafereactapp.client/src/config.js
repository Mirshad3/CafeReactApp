import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

// Configuration options (modify these as needed)
const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;
const certificateName = "cafereactapp.client";  // Change this if needed
const apiUrl =  env.REACT_APP_API_URL;  
const environment = env.REACT_APP_ENV;  
// Ensure base folder exists
if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

// Certificate and key file paths
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// Create certificate and key if missing (improved error handling)
if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    try {
        child_process.spawnSync('dotnet', [
            'dev-certs',
            'https',
            '--export-path',
            certFilePath,
            '--format',
            'Pem',
            '--no-password',
        ], { stdio: 'inherit' });
    } catch (error) {
        console.error("Failed to create certificate:", error.message);
        //process.exit(1); // Exit with an error code
    }
}

// Target URL for API proxy (modify as needed)
const target = env.ASPNETCORE_HTTPS_PORT
    ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
    : env.ASPNETCORE_URLS
        ? env.ASPNETCORE_URLS.split(';')[0]
        : 'https://localhost:7188';

// Vite configuration
export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        },
    },
    server: {
        proxy: {

            '^/weatherforecast': {

                target,

                secure: false

            }

        },
     
        port: 60462,
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        },
    },
    // Optional configuration for API endpoint and environment variables
    ...(apiUrl && { define: { 'process.env.REACT_APP_API_URL': apiUrl } }),
    ...(environment && { define: { 'process.env.REACT_APP_ENV': environment } }),
});