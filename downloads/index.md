---
title: Downloads
layout: default
---
# {{ page.title }}
{% for version in site.data.downloads %}
## {{ site.project.name }} {{ version.name }}

| Name                  | Download-Link                                 |
|-----------------------|-----------------------------------------------|{% for download in version.downloads %}
| {{ download.name }}   | [{{ download.filename }}]({{ download.url }}) |{% endfor %}{% endfor %}