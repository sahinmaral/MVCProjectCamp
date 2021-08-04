var disableAbout = function (aboutHeaderForFriendlyUrl) {

    if (aboutHeaderForFriendlyUrl) {
        swal({
            text: "Herhangi bir hakkımızda yazısı kalmayacağı için birinci olan hakkımızda yazısı aktif olacaktır. Onaylıyor musunuz ? ",
            icon: "warning",
            buttons: {
                accepted: { text: "Evet", value: true },
                notAccepted: { text: "Hayır", value: false }
            }
        }).then((value) => {
            if (value == true) {
                window.location.replace('/AdminAbouts/EnableAbout?aboutHeaderForFriendlyUrl=' + aboutHeaderForFriendlyUrl);
            }
        });
    }
}