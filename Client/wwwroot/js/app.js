


window.clipboardCopy = {
    copyText: function (text) {
        navigator.clipboard.writeText(text).then(function () {
            alert("Referral link is copied to clipboard 😄");
        })
            .catch(function (error) {
                alert(error);
            });
    }
};