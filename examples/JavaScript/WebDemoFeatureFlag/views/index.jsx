var React = require('react');

/* Uses feature flag to display link to "beta" area */
function NavigationBar({beta}) {

    return (
        <container>
            <header className="d-flex justify-content-center py-3">
                <ul className="nav nav-pills">
                    <li className="nav-item"><a href="#Home" className="nav-link active" aria-current="page">Home</a></li>
                    {(beta===true) && <li className="nav-item"><a href="#Beta" className="nav-link">Beta</a></li>}
                    <li className="nav-item"><a href="#Privacy" className="nav-link">Privacy</a></li>
                </ul>
            </header>
        </container>
    );
}
function Index(props) {

    return (
        <DefaultLayout >
            <NavigationBar beta={props.beta}></NavigationBar>
            <container>
                <div className="p-5 mb-4 bg-light rounded-3 display-5 fw-bold text-center">Welcome</div>
            </container>
        </DefaultLayout>
    );
}
function DefaultLayout(props) {
    return (
        <html>
            <head>
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3" crossOrigin="anonymous" />
            </head>
            <body>
                <main>
                    {props.children}
                </main>
                <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
            </body>
        </html>
    );
}

module.exports = Index;