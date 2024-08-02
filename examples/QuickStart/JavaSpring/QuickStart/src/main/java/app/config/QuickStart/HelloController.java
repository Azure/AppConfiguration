package app.config.QuickStart;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class HelloController {
    private final MyProperties properties;

    public HelloController(MyProperties properties) {
        this.properties = properties;
    }

    @GetMapping
    public String getMessage() {
        return "Message: " + properties.getMessage();
    }
}
