(function () {
    window.addEventListener("load", function () {
        setTimeout(function () {
            var logo = document.getElementsByClassName('link')[0];
            if (logo) {
                logo.href = "https://www.forca.dk";
                logo.target = "_blank";

                var svgLogo = logo.children[0];
                if (svgLogo && svgLogo.tagName === "svg") {
                    // Replace SVG with an IMG element
                    var imgLogo = document.createElement("img");
                    imgLogo.alt = "FORCA Swagger";
                    imgLogo.src = "/swagger-ui/ForcaLogo.png";
                    imgLogo.style.height = "30px";
                    logo.replaceChild(imgLogo, svgLogo);
                }
            } else {
                console.error("Logo element not found");
            }
        }, 30);
    });
})();
