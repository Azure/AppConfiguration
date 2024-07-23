package app.config.QuickStart;

import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;

@SpringBootTest(properties = "spring.cloud.azure.appconfiguration.enabled=false")
class QuickStartApplicationTests {

	@Test
	void contextLoads() {
	}

}