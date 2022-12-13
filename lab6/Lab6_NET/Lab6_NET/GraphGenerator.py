from os import path
from sys import argv, stdout

import matplotlib.pyplot as plt
import networkx as nx


# zwraca warstwy FNF z pliku .tmp
def GetFnf(file):
    fl = open(file, "r")
    lines, fnf, = fl.readlines(), []

    for layer in lines:
        edges, fnfLayer = layer.split(";"), []
        for edge in edges:
            verts = edge.split(".")
            if len(verts) != 2:
                continue
            fnfLayer.append((verts[0], verts[1]))
        fnf.append(fnfLayer)

    return fnf


# funkcja rysuje graf i zapisuje go do pliku
def Plot(g, file):
    G, color, i = nx.Graph(), {"S!": 0}, 0

    for layer in g:
        for edge in layer:
            color.update({edge[0]: i / len(g)})
            color.update({edge[1]: (i + 1) / len(g)})
            G.add_edge(edge[0], edge[1])
        i += 1

    nx.draw(
        G,
        node_color=[color.get(node, 0.01) for node in G.nodes()],
        with_labels=True,
        font_size=5,
        cmap=plt.get_cmap("cool"),
    )
    plt.savefig(file, dpi=200)


# # # # # # # # MAIN # # # # # # #
try:
    if len(argv) != 3:
        stdout.write("Nie podano ścieżek do plików!")
        exit(2)

    inFile, imgFile = argv[1], argv[2]
    Plot(GetFnf(inFile), imgFile)
    stdout.write(f"Graf zapisano do: '{imgFile}'")
    exit(0)
except Exception as e:
    print(e)
