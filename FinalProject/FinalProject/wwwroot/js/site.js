// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let apiURL = "https://forkify-api.herokuapp.com/api/v2/recipes";
let apikey = "415c4809-fb2d-47e5-a7da-9b58e22ef8df";

//Gets recipes based on a name, limits display.
//Calls the Forkify API to get recipes based on the provided recipeName.
//Limits by isAllShow parameter and Calls the showRecipes function to display the retrieved recipes in the specified HTML element(id).
async function GetRecipes(recipeName, id, isAllShow) {
    let resp = await fetch(`${apiURL}?search=${recipeName}&key=${apikey}`);
    let result = await resp.json();
    let Recipes = isAllShow ? result.data.recipes : result.data.recipes.slice(1, 7);
    showRecipes(Recipes, id);
}

//Displays recipes in an HTML element, updates cart icons.
//Makes an AJAX POST request to the server(/Recipe/GetRecipeCard) with the retrieved recipes as JSON data.
//Updates the HTML content of the specified element (id) with the response, which likely contains the recipe cards.
//Calls the getAddedCarts function after updating the HTML.
function showRecipes(recipes, id) {
    $.ajax({
        contentType: "application/json; charset=uft-8",
        dataType: 'html',
        type: 'POST',
        url: '/Recipe/GetRecipeCard',
        data: JSON.stringify(recipes),
        success: function (htmlResult) {
            $('#' + id).html(htmlResult);
            getAddedCarts();
        }
    });
}

//Gets details for a specific recipe.
//Calls the Forkify API to get details for a specific recipe based on the provided id.
//Calls the showOrderRecipeDetails function to display the recipe details in the specified HTML element(showId).
async function getOrderRecipe(id, showId) {

    let resp = await fetch(`${apiURL}/${id}?key=${apikey}`);
    let result = await resp.json();
    let recipe = result.data.recipe;
    showOrderRecipeDetails(recipe, showId);

}

//Displays recipe details in an HTML element.
//Makes an AJAX POST request to the server(/Recipe/ShowOrder) with the order recipe details as data.
//Updates the HTML content of the specified element (showId) with the response.
function showOrderRecipeDetails(orderRecipeDetails, showId) {
    $.ajax({
        url: '/Recipe/ShowOrder',
        data: orderRecipeDetails,
        dataType: 'html',
        type: 'POST',
        success: function (htmlResult) {
            $('#' + showId).html(htmlResult);
        }
    });
}

//Order Page


//Handles quantity changes on the order page.
//Adjusts the quantity and total amount based on the provided option ('inc' for increase, 'dec' for decrease).
//Updates the corresponding input fields.
function quantity(option) {
    let qty = $('#qty').val();
    let price = parseInt($('#price').val());
    let totalAmount = 0;
    if (option == 'inc') {
        ;
        qty = parseInt(qty) + 1;
    }
    else {
        qty = qty == 1 ? qty : qty - 1;
    }
    totalAmount = price * qty;
    $('#qty').val(qty);
    $('#totalAmount').val(totalAmount);
}


//Adds/removes recipes from the cart, updates icons.
//Makes AJAX POST requests to the server for adding or removing a recipe from the cart.
//Calls the getAddedCarts function after the operation.
async function cart() {
    let iTag = $(this).children('i')[0];
    let recipeId = $(this).attr('data-recipeId');
    if ($(iTag).hasClass('fa-regular')) {
        let resp = await fetch(`${apiURL}/${recipeId}?key=${apikey}`);
        let result = await resp.json();
        let cart = result.data.recipe;
        cart.RecipeId = recipeId;
        delete cart.id;
        cartRequest(cart, 'SaveCart', 'fa-solid', 'fa-regular', iTag, false);
    }
    else {
        let data = { Id: recipeId };
        cartRequest(data, 'RemoveCartFromList', 'fa-regular', 'fa-solid', iTag, false);
    }
}

//Handles AJAX requests related to the cart.
//Handles success and error cases.
function cartRequest(data, action, addcls, removecls, iTag, isReload) {
    $.ajax({
        url: '/Cart/' + action,
        type: 'POST',
        data: data,
        success: function (resp) {
            if (isReload) {
                location.reload();
            }
            else {
                $(iTag).addClass(addcls);
                $(iTag).removeClass(removecls);
            }

        },
        error: function (err) {
            console.log(err);
        }
    });
}

//Updates icons based on the user's cart.
//Retrieves the list of recipe IDs in the user's cart and updates the corresponding icons on the page.
function getAddedCarts() {
    $.ajax({
        url: '/Cart/GetAddedCarts',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            $('.addToCartIcon').each((index, spanTag) => {
                let recipeId = $(spanTag).attr("data-recipeId");
                for (var i = 0; i < result.length; i++) {
                    if (recipeId == result[i]) {
                        let itag = $(spanTag).children('i')[0];
                        $(itag).addClass('fa-solid');
                        $(itag).removeClass('fa-regular');
                        break;
                    }
                }
            })
        },
        error: function (err) {
            console.log(err);
        }
    });

}

//Retrieves and updates the user's cart list.
//Makes an AJAX GET request to retrieve the HTML content of the user's cart list.
//Updates the HTML content of the specified element(#showCartList).
function getCartList() {
    $.ajax({
        url: 'Cart/GetCartList',
        type: 'GET',
        dataType: 'html',
        success: function (result) {
            $('#showCartList').html(result);
        },
        error: function (err) {
            console.log(err);
        }
    });
}

//Removes a recipe from the user's cart.
//Calls the cartRequest function to handle the removal.
function removeCartfromList(id) {
    console.log(id);
    let data = { Id: id };
    cartRequest(data, 'RemoveCartFromList', null, null, null, true);
}
