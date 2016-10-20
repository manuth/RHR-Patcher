(function ($)
{
    $(document).ready(function ()
    {
        $("pre").each(function ()
        {
            var pre = $(this).text().split("\n");
            var lines = new Array(pre.length+1);
            for (var i = 0; i < pre.length; i++)
            {
                if (pre[i] == "" && i == pre.length - 1)
                    lines.splice(i, 1);
                else
                    lines[i] = i + 1;
            }
            $(this).before('<pre class="lines"><div>' + lines.join("</div>\n<div>") + "</div></pre>");

            var linesContainer = $(this).prev();

            linesContainer.css("text-align", "right");
            linesContainer.css("position", "relative");
            linesContainer.css("margin-left", $(this).css("margin-left"));
            linesContainer.css("margin-right", linesContainer.outerWidth() * -1);
            linesContainer.css("background", $(this).css("background"));
            $(this).css("padding-left", linesContainer.outerWidth() + 10);
        });
    });
})(jQuery)