import cv2
import urllib
import numpy as np

class calculator:
    def add(self, x, y):
        return x + y

    def increment(self, x):
        x += 1;
        return x;

    def image(self, url):
        req = urllib.urlopen(url)
        arr = np.asarray(bytearray(req.read()), dtype=np.uint8)
        img = cv2.imdecode(arr, -1) # 'Load it as it is'
        return img.shape
