# --- Day 9: Smoke Basin ---

These caves seem to be lava tubes. Parts are even still volcanically active; small hydrothermal vents release smoke into the caves that slowly settles like rain.

If you can model how the smoke flows through the caves, you might be able to avoid it and be that much safer. The submarine generates a heightmap of the floor of the nearby caves for you (your puzzle input).

Smoke flows to the lowest point of the area it's in. For example, consider the following heightmap:

![aoc9-1](https://user-images.githubusercontent.com/22420744/145483945-8e5a203a-724d-47f5-a1ce-04ab9043fa86.png)


Each number corresponds to the height of a particular location, where 9 is the highest and 0 is the lowest a location can be.

Your first goal is to find the low points - the locations that are lower than any of its adjacent locations. Most locations have four adjacent locations (up, down, left, and right); locations on the edge or corner of the map have three or two adjacent locations, respectively. (Diagonal locations do not count as adjacent.)

In the above example, there are four low points, all highlighted: two are in the first row (a 1 and a 0), one is in the third row (a 5), and one is in the bottom row (also a 5). All other locations on the heightmap have some lower adjacent location, and so are not low points.

The risk level of a low point is 1 plus its height. In the above example, the risk levels of the low points are 2, 1, 6, and 6. The sum of the risk levels of all low points in the heightmap is therefore 15.

Find all of the low points on your heightmap. What is the sum of the risk levels of all low points on your heightmap?


# --- Part Two ---

Next, you need to find the largest basins so you know what areas are most important to avoid.

A basin is all locations that eventually flow downward to a single low point. Therefore, every low point has a basin, although some basins are very small. Locations of height 9 do not count as being in any basin, and all other locations will always be part of exactly one basin.

The size of a basin is the number of locations within the basin, including the low point. The example above has four basins.

The top-left basin, size 3:

![aoc9-2](https://user-images.githubusercontent.com/22420744/145483963-11032592-5c48-4cbb-9ffc-c7fcf1ac6c89.png)



The top-right basin, size 9:

![aoc9-3](https://user-images.githubusercontent.com/22420744/145483968-315ef22b-8a6c-4748-a4ac-0dc1bdec753d.png)



The middle basin, size 14:


![aoc9-4](https://user-images.githubusercontent.com/22420744/145483973-9b4e9eb7-5a9c-4ed9-a5a5-231186dec5e1.png)


The bottom-right basin, size 9:

![aoc9-5](https://user-images.githubusercontent.com/22420744/145483977-d9453b08-1262-47d1-8e17-9f6c8b20f460.png)


Find the three largest basins and multiply their sizes together. In the above example, this is 9 * 14 * 9 = 1134.

What do you get if you multiply together the sizes of the three largest basins?
