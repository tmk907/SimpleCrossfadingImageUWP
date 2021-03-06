﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SimpleCrossfadingImageUWP
{
    public sealed partial class CrossfadingImage : UserControl
    {
        public CrossfadingImage()
        {
            this.InitializeComponent();
            imageTop.ImageOpened += ImageTop_ImageOpened;
            imageBottom.ImageOpened += ImageBottom_ImageOpened;
        }

        private void ImageBottom_ImageOpened(object sender, RoutedEventArgs e)
        {
            BottomImageShow.Begin();
            TopImageHide.Begin();
        }

        private void ImageTop_ImageOpened(object sender, RoutedEventArgs e)
        {
            TopImageShow.Begin();
            BottomImageHide.Begin();
        }

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set
            {
                SetValue(StretchProperty, value);
                OnStretchChanged();
            }
        }

        private void OnStretchChanged()
        {
            imageBottom.Stretch = Stretch;
            imageTop.Stretch = Stretch;
        }

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(CrossfadingImage), null);

        public Uri ImageUri
        {
            get { return (Uri)GetValue(ImageUriProperty); }
            set
            {
                SetValue(ImageUriProperty, value);
                OnImageUriChange();
            }
        }

        public static readonly DependencyProperty ImageUriProperty =
            DependencyProperty.Register("ImageUri", typeof(Uri), typeof(CrossfadingImage), null);

        private void OnImageUriChange()
        {
            if (ImageUri == null) return;

            BitmapImage bmp;
            if (DecodePixelHeight != 0 && DecodePixelWidth != 0)
            {
                bmp = new BitmapImage();
                bmp.DecodePixelHeight = DecodePixelHeight;
                bmp.DecodePixelWidth = DecodePixelWidth;
                bmp.DecodePixelType = DecodePixelType.Logical;
                bmp.UriSource = ImageUri;
            }
            else
            {
                bmp = new BitmapImage(ImageUri);
            }

            if (imageTop.Opacity == 0)
            {
                imageTop.Source = bmp;
            }
            else
            {
                imageBottom.Source = bmp;
            }
        }

        public int DecodePixelHeight
        {
            get { return (int)GetValue(DecodePixelHeightProperty); }
            set { SetValue(DecodePixelHeightProperty, value); }
        }

        public static readonly DependencyProperty DecodePixelHeightProperty =
            DependencyProperty.Register("DecodePixelHeight", typeof(int), typeof(int), new PropertyMetadata(0));

        public int DecodePixelWidth
        {
            get { return (int)GetValue(DecodePixelWidthProperty); }
            set { SetValue(DecodePixelWidthProperty, value); }
        }

        public static readonly DependencyProperty DecodePixelWidthProperty =
            DependencyProperty.Register("DecodePixelWidth", typeof(int), typeof(int), new PropertyMetadata(0));


        public TimeSpan FadeInDuration
        {
            get { return (TimeSpan)GetValue(FadeInDurationProperty); }
            set
            {
                SetValue(FadeInDurationProperty, value);
                TopImageShowAnimation.Duration = value;
                BottomImageShowAnimation.Duration = value;
            }
        }

        // Using a DependencyProperty as the backing store for FadeInDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FadeInDurationProperty =
            DependencyProperty.Register("FadeInDuration", typeof(TimeSpan), typeof(CrossfadingImage), new PropertyMetadata(TimeSpan.FromSeconds(1)));


        public TimeSpan FadeOutDuration
        {
            get { return (TimeSpan)GetValue(FadeOutDurationProperty); }
            set
            {
                SetValue(FadeOutDurationProperty, value);
                TopImageHideAnimation.Duration = value;
                BottomImageHideAnimation.Duration = value;
            }
        }

        // Using a DependencyProperty as the backing store for FadeOutDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FadeOutDurationProperty =
            DependencyProperty.Register("FadeOutDuration", typeof(TimeSpan), typeof(CrossfadingImage), new PropertyMetadata(TimeSpan.FromSeconds(1)));

    }
}
