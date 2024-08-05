package app.config.QuickStart;
import org.springframework.boot.context.properties.ConfigurationProperties;

// The `prefix` value of `@ConfigurationProperties` matches the created key of `/application/config.message` so all properties with that prefix are added to this `@ConfigurationProperties`
// Note: By default, `/application/` is removed from the key name.
@ConfigurationProperties(prefix = "config")
public class MyProperties {
    private String message;

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }
}
