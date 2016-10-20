---
layout: default
title: Dokumente
---
{% include base.html %}
<div class="rhrpatcher-banner"><img src="{{ page.base }}/assets/images/Banner.Image.png" /></div>

# {{ page.title }}

Folgende Auflistung beinhaltet sämtliche auf dieser Seite vorhandenen Dokumente.  
Darunter befinedn sich, von der Dokumentation abgesehen, auch Notizen, Reminder oder Präsentationen.

{% for section in site.data.docs %}
* {% assign section_url = "/docs/" | append: section.path | append: "/index.html" %}{% assign page_urls = site.docs | map: "url" %}{% if page_urls contains section_url %}[{{ section.title }}]({{ section_url }}){% else %}{{ section.title }}{% endif %}{% for doc_name in section.docs %}{% assign doc_url = doc_name | prepend: "/" | prepend: section.path | prepend: "/docs/" | append: ".html" %}{% assign doc = site.docs | where: "url", doc_url | first %}
    * [{{ doc.title }}]({{ base }}{{ doc.url }}){% endfor %}{% endfor %}