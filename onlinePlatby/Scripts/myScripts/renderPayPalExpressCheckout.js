var finalPrice = $("#finalPriceHolder").attr("data-finalPrice");
finalPrice = finalPrice.replace(/\,/g, '.');
finalPrice = finalPrice.substring(0, finalPrice.length - 1);
finalPrice = finalPrice.substring(0, finalPrice.length - 1);
var name = $("#nameHolder").attr("data-name");
var email = $("#emailHolder").attr("data-email");
paypal.Button.render({

    env: 'sandbox', // Optional: specify 'sandbox' environment

    client: {
        sandbox: 'Adq8yD0SPsOlUwWaDkrV0bGihpZsW2fXnXqTiu98WBCEoOFy12b1CBaXKwm8FGqVgigHTmzNWOGmaPbx',
        production: 'xxxxxxxxx'
    },

    payment: function () {

        var env = this.props.env;
        var client = this.props.client;


        return paypal.rest.payment.create(env, client, {
            transactions: [
                {
                    amount: { total: finalPrice, currency: 'CZK' }
                }
            ]
        });
    },

    commit: true, // Optional: show a 'Pay Now' button in the checkout flow

    onAuthorize: function (data, actions) {

        // Optional: display a confirmation page here

        return actions.payment.execute().then(function () {
            $.ajax({
                url: "/Basket/OrderSuccess?name=" + name + "&email=" + email + "&finalPrice=" + finalPrice,
                method: "get",
                success: function (data) {
                    window.location.replace("/Basket/OrderIsSuccessful");
                }
            });
        });
    }

}, '#paypal-button');