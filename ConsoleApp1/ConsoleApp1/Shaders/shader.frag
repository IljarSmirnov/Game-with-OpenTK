#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec3 ReflectDir;

uniform vec4 uColor;
uniform vec3 uLightPos;
uniform vec3 uViewPos;
uniform int uUseLighting;
uniform float uSpecularStrength;
uniform float uShininess;
uniform samplerCube uCubeMap;

void main()
{
    if (uUseLighting == 1) {
        vec3 lightColor = vec3(1.0, 1.0, 1.0);
        float specularStrength = 1.0;
        float shininess = 32.0;
        vec3 norm = normalize(Normal);
        vec3 lightDir = normalize(uLightPos - FragPos);
        vec3 viewDir = normalize(uViewPos - FragPos);
        vec3 halfwayDir = normalize(lightDir + viewDir);
        float spec = pow(max(dot(norm, halfwayDir), 0.0), shininess);
        vec3 specular = specularStrength * spec * lightColor;
        vec3 result = specular + uColor.rgb * 0.7;
        FragColor = vec4(result, uColor.a);
    } else if (uUseLighting == 2) {
        vec3 lightColor = vec3(1.0, 1.0, 1.0);
        float specularStrength = 0.3;
        float shininess = 16.0;
        vec3 norm = normalize(Normal);
        vec3 lightDir = normalize(uLightPos - FragPos);
        vec3 viewDir = normalize(uViewPos - FragPos);
        vec3 halfwayDir = normalize(lightDir + viewDir);
        float diff = max(dot(norm, lightDir), 0.0);
        vec3 diffuse = diff * lightColor * uColor.rgb;
        float spec = pow(max(dot(norm, halfwayDir), 0.0), shininess);
        vec3 specular = specularStrength * spec * lightColor;
        vec3 ambient = 0.3 * uColor.rgb;
        vec3 result = ambient + diffuse + specular;
        FragColor = vec4(result, uColor.a);
    } else if (uUseLighting == 3) {
        vec3 envColor = texture(uCubeMap, ReflectDir).rgb;
        vec3 viewDir = normalize(uViewPos - FragPos);
        vec3 norm = normalize(Normal);
        vec3 lightDir = normalize(uLightPos - FragPos);
        vec3 halfwayDir = normalize(lightDir + viewDir);
        float spec = pow(max(dot(norm, halfwayDir), 0.0), uShininess);
        vec3 specular = uSpecularStrength * spec * vec3(1.0);
        FragColor = vec4(envColor *0.85 + specular, uColor.a);
    } else {
        FragColor = uColor;
    }
}