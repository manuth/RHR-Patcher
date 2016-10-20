function animateCarouselHeight()
{
    $(".carousel").on("slide.bs.carousel", function(a)
    {
        var b = $(a.relatedTarget).height();
        $(this).find(".active.item").parent().height() - b;
        $(this).find(".active.item").parent().animate(
        {
            height: b
        }, 500)
    })
}

$(document).ready(function()
{
    $.minicolors.defaults.theme = "bootstrap", $("input.minicolors").minicolors
    ({
        control: $(this).attr("data-control") || "hue",
        defaultValue: $(this).attr("data-defaultValue") || "",
        format: $(this).attr("data-format") || "hex",
        keywords: $(this).attr("data-keywords") || "",
        inline: "true" === $(this).attr("data-inline"),
        letterCase: $(this).attr("data-letterCase") || "uppercase",
        opacity: $(this).attr("data-opacity") || 1,
        position: $(this).attr("data-position") || "bottom left",
        swatches: $(this).attr("data-swatches") ? $(this).attr("data-swatches").split("|") : []
    });
    animateCarouselHeight();
    $('.disabled a, .disabled button, .disabled input[type="button"]').click(function(event){ event.preventDefault(); });
    $('.disabled a, .disabled button, .disabled input[type="button"]').submit(function(event){ event.preventDefault(); });
});